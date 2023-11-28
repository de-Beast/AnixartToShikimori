namespace AnixartToShikimori.CSVConverter;

public record AnimeTitle
{
	public string TargetTitle { get; init; }
	public int TargetId { get; init; }
	public string TargetType => "Anime";
	public int Score { get; init; }
	public string Status { get; init; }
	public int Rewatches { get; init; }
	public int Episodes { get; init; }
	public string? Text => null;
}

public class Converter
{
	public string FilePath { get; private set; }

	public Converter(string file_path)
	{
		FilePath = file_path ?? throw new ArgumentNullException(nameof(file_path));

		FileInfo file_info = new (file_path);
		if (!file_info.Exists)
		{
			throw new FileNotFoundException($"File not found on that path {file_path}");
		}

		if (file_info.Extension != ".csv")
		{
			throw new FileLoadException("File format must be .csv");
		}
	}

	public (List<AnimeTitle>, List<string>) ExtractFromFile()
	{
		using StreamReader reader = new(FilePath);
		List<AnimeTitle> animes = new();
		List<string> failed_animes = new();
		reader.ReadLine();
		while (!reader.EndOfStream)
		{
			string? line = reader.ReadLine();
			var values = line?.Split(',');
			if (values is null) continue;
			try
			{
				animes.Add(new()
				{
					TargetTitle = values[2].Trim('"'),
					TargetId = default,
					Rewatches = default,
					Episodes = default,
					Status = values[5],
					Score = int.Parse(values[^1][0].ToString()) * 2
				});
			}
			catch (IndexOutOfRangeException e)
			{
				failed_animes.Add(values[2].Trim('"'));
			}
		}

		return (animes, failed_animes);
	}
}