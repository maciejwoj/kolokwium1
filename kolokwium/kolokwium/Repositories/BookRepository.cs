using System.Data.SqlClient;
using kolokwium.Models.DTOs;

namespace kolokwium.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly IConfiguration _configuration;
    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<bool> DoesBookExist(int id)
    {
        var query = "SELECT 1 FROM books WHERE PK = @ID";
 
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand(query, connection);
 
        command.Parameters.AddWithValue("@ID", id);
 
        await connection.OpenAsync();
 
        var result = await command.ExecuteScalarAsync();
 
        return result != null;
    }

    public async Task<List<string>> GetGenresBook(int id)
    {
        var query = @"SELECT genres.name
                    FROM books
                    JOIN books_genres AS bg ON bg.FK_book = books.PK
                    JOIN genres ON genres.PK = bg.FK_genre
                    WHERE books.PK = @ID";

        List<string> genres = new List<string>();

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();
        var genresNameOrdinal = reader.GetOrdinal("name");

        while (await reader.ReadAsync())
        {
            genres.Add(reader.GetString(genresNameOrdinal));
        }

        return genres;
    }

    public async Task AddBookWithGenres(NewBookWithGenresDTO newBookWithGenresDto)
    {
        var insert = @"INSERT INTO Animal VALUES(@title);
					   SELECT @@IDENTITY AS PK;";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
	    
        command.Connection = connection;
        command.CommandText = insert;
        
        command.Parameters.AddWithValue("@title", newBookWithGenresDto.Title);
        
        await connection.OpenAsync();
        
        var transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;
        
        try
        {
            var id = await command.ExecuteScalarAsync();
    
            foreach (var genre in newBookWithGenresDto.Genres)
            {
                command.Parameters.Clear();
                command.CommandText = "INSERT INTO books_genres VALUES(@FK_book, @FK_genre)";
                
                command.Parameters.AddWithValue("@FK_book", newBookWithGenresDto.PK);
                command.Parameters.AddWithValue("@FK_genre", genre);


                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
}