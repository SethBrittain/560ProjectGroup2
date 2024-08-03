using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.IO;

namespace Pidgin.Controller;

[Controller]
[Route("upload")]
public class FileUploadController : ControllerBase {
	private const int MAX_FILE_SIZE = 5 * 1024 * 1024; // 5 MB
	private const int MAX_ACCEPTABLE_SIZE = 100 * 1024 * 1024; // 100 MB
	private readonly IWebHostEnvironment _env;
	private readonly NpgsqlDataSource _dataSource;
	public FileUploadController(IWebHostEnvironment env, NpgsqlDataSource dataSource)
	{
		_env = env;
		_dataSource = dataSource;
	}

	[HttpPost("profile")]
	[Authorize(AuthenticationSchemes = "Cookies")]
	public async Task<IActionResult> UploadFile(IFormFile file)
	{
		int uid = int.Parse(HttpContext.User.FindFirstValue("uid"));
		if (file == null)
			return BadRequest("No file was uploaded");
		if (file.Length == 0)
			return BadRequest("File is empty");
		if (file.Length > MAX_FILE_SIZE)
			return BadRequest("File is too large");
		
		var allowedFileTypes = new[] { "image/jpeg", "image/png", "image/gif" };
		if (!allowedFileTypes.Contains(file.ContentType))
		{
			return BadRequest("Only JPEG and PNG files are allowed.");
		}

		string ext;
		switch (file.ContentType)
		{
			case "image/jpeg":
				ext=".jpg";
				break;
			case "image/png":
				ext=".png";
				break;
			default:
				return BadRequest("failed");
		}

		Guid guid = Guid.NewGuid();

		string fileName = guid.ToString() + ext;
		string filePath = Path.Combine(_env.WebRootPath, "static", "img", "profile", fileName);

		await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);

		string oldFileSql = @"
			SELECT profile_photo
			FROM users
			WHERE user_id = @uid
		";
		await using NpgsqlCommand oldFileCommand = _dataSource.CreateCommand(oldFileSql);
		oldFileCommand.Parameters.AddWithValue("uid", uid);
		await using NpgsqlDataReader reader = await oldFileCommand.ExecuteReaderAsync();
		
		if (await reader.ReadAsync())
		{
			string sql = @"
				UPDATE users
				SET profile_photo = @fileName
				WHERE user_id = @uid
			";
			await using NpgsqlCommand command = _dataSource.CreateCommand(sql);
			command.Parameters.AddWithValue("fileName", Path.Join("/static/img/profile/",fileName));
			command.Parameters.AddWithValue("uid", uid);

			if (!reader.IsDBNull(0))
				System.IO.File.Delete(Path.Join(_env.WebRootPath, reader.GetString(0)));
			
			if (await command.ExecuteNonQueryAsync() != 1) 
				throw new Exception("Failed to update profile picture");
		}
		else
		{
			return BadRequest("Failed to update profile picture");
		}

		return Ok();
	}
}