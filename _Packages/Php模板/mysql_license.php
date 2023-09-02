<?php

$configFilePath = "C:/xxx/mysql_license.txt";
$configLines = file($configFilePath, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
if (count($configLines) < 5) { 
    die("Config file format is incorrect.");
}

$hostname = $configLines[0];
$username = $configLines[1];
$password = $configLines[2];
$database = $configLines[3];
$key = trim($configLines[4]);

function decrypt($data, $key) {
    $encryptedData = base64_decode($data);
    $iv = str_repeat(chr(0), 16);
    $decrypted = openssl_decrypt($encryptedData, 'aes-256-cbc', $key, OPENSSL_RAW_DATA, $iv);
    return $decrypted;
}

$connection = new mysqli($hostname, $username, $password, $database);
if ($connection->connect_error) {
    die("Connection failed: " . $connection->connect_error);
}

if (!isset($_POST['sqlQuery'])) {
    die("No request received.");
}

$encryptedRequest = $_POST['sqlQuery'];
$decodedRequest = decrypt($encryptedRequest, $key);
$data = json_decode($decodedRequest, true);

$response = array();

if ($data && isset($data["command"])) {
    $commands = explode(';', $data["command"]);

    // Ensure params is always an array
    $params = isset($data["params"]) ? (is_array($data["params"]) ? $data["params"] : array($data["params"])) : array();
    $currentParamIndex = 0;

    foreach ($commands as $command) {
        if (trim($command) === "") continue; // Skip empty commands

        $paramCount = substr_count($command, '?');
        $commandParams = array_slice($params, $currentParamIndex, $paramCount);
        $currentParamIndex += $paramCount;

        $stmt = $connection->prepare($command);
        if ($stmt) {
            if ($paramCount > 0) {
                $types = str_repeat('s', $paramCount);
                $stmt->bind_param($types, ...$commandParams);
            }
            
            if ($stmt->execute()) {
                $result = $stmt->get_result();
                if ($result) {
                    while ($row = $result->fetch_assoc()) {
                        $response[] = $row;
                    }
                    $result->free();
                }
            } else {
                $response["error"] = "MySQL command failed: " . $stmt->error;
            }
            $stmt->close();
        } else {
            $response["error"] = "MySQL preparation failed: " . $connection->error;
        }
    }
} else {
    $response["error"] = "Invalid request format.";
}

echo json_encode($response);

?>

