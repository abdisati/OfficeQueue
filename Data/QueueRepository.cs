using Microsoft.Data.Sqlite;
using OfficeQueue.Models;

namespace OfficeQueueBot.Data;

public class QueueRepository
{
    private readonly string _connectionString = "Data Source=queue.db";

    public QueueRepository()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS Queue (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            TelegramId INTEGER,
            Name TEXT,
            Service TEXT,
            QueueNumber INTEGER,
            Status TEXT,
            CreatedAt TEXT
        )";
        tableCmd.ExecuteNonQuery();
    }

    public int AddToQueue(QueueEntry entry)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Find current max queue number for this service
        var maxCmd = connection.CreateCommand();
        maxCmd.CommandText = "SELECT IFNULL(MAX(QueueNumber), 0) FROM Queue WHERE Service = $service";
        maxCmd.Parameters.AddWithValue("$service", entry.Service);
        var maxNumber = Convert.ToInt32(maxCmd.ExecuteScalar());

        entry.QueueNumber = maxNumber + 1;

        var insertCmd = connection.CreateCommand();
        insertCmd.CommandText =
        @"INSERT INTO Queue (TelegramId, Name, Service, QueueNumber, Status, CreatedAt)
          VALUES ($tid, $name, $service, $qnum, $status, $created)";
        insertCmd.Parameters.AddWithValue("$tid", entry.TelegramId);
        insertCmd.Parameters.AddWithValue("$name", entry.Name);
        insertCmd.Parameters.AddWithValue("$service", entry.Service);
        insertCmd.Parameters.AddWithValue("$qnum", entry.QueueNumber);
        insertCmd.Parameters.AddWithValue("$status", entry.Status);
        insertCmd.Parameters.AddWithValue("$created", entry.CreatedAt.ToString("o"));
        insertCmd.ExecuteNonQuery();

        return entry.QueueNumber;
    }
}
