<?php

// 读取配置文件
$configFilePath = "C:/xxx/mysql_xxx.txt";
$configLines = file($configFilePath, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
if (count($configLines) < 5) { // 确保配置文件有5行数据
    die("Config file format is incorrect.");
}

//在服务器上,通常指 localhost
$hostname = $configLines[0];
$username = $configLines[1];
$password = $configLines[2];
$database = $configLines[3];
$key = trim($configLines[4]);  // 从文件中读取key

// AES解密函数
function decrypt($data, $key) {
    $encryptedData = base64_decode($data);
    $iv = str_repeat(chr(0), 16); // 和Unity中的初始化向量一致
    $decrypted = openssl_decrypt($encryptedData, 'aes-256-cbc', $key, OPENSSL_RAW_DATA, $iv);
    return $decrypted;
}

// 创建或重新连接到数据库
function connectToDatabase($hostname, $username, $password, $database) {
    $connection = new mysqli($hostname, $username, $password, $database);
    if ($connection->connect_error) {
        die("Connection failed: " . $connection->connect_error);
    }
    return $connection;
}

// 连接到数据库
$connection = connectToDatabase($hostname, $username, $password, $database);

// 检查是否收到了来自Unity的请求
if (!isset($_POST['sqlQuery'])) {
    die("No request received.");
}

$encryptedRequest = $_POST['sqlQuery'];

// 解密请求
$request = decrypt($encryptedRequest, $key);

// 检查连接是否仍然活动
if ($connection->ping() === false) {
    $connection = connectToDatabase($hostname, $username, $password, $database); // 重新连接
}

$response = array();

if (!empty($request)) {
    if ($connection->multi_query($request)) {
        do {
            if ($result = $connection->store_result()) {
                while ($row = $result->fetch_assoc()) {
                    $response[] = $row;
                }
                $result->free();
            }
        } while ($connection->more_results() && $connection->next_result());
    } else {
        $response["error"] = "MySQL command failed: " . $connection->error;
    }
} else {
    $response["error"] = "Empty request received.";
}

// 将响应转为JSON格式并返回给Unity
echo json_encode($response);

?>
