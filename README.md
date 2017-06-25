# LexDbf
Инструментарий для работы c DBF файлами. Главным образом под VisualStudio 2008
Предоставляется возможность чтения и записи DBF файлов

Возможна работа в различных парадигмах
1. 
	using(var dbf = DbfReader.Open("test.dbf"))
	{
		while(dbf.Read())
		{
			var a = dbf[1];
			var b = dbf["COL"];
		}
	}
	
2.
	using(var dbf = DbfReader.Open("test.dbf"))
	{
		var dt = new DataTable();
		dbf.Fill(dt);
	}
	
3. Рекомендуемый
	using(var dbf = DbfReader.Open("test.dbf"))
	{
		var data = dbf.GetBody<TESTDBF>();
	}
	Где производиться типизированный мапинг на класс List<TESTDBF>
	Есть утилита, которая позволяет по известной структуре генерировать данный тип классов
	В дальнейшем это будет VSPackage для VS
	
Запись в файл производиться в следующем виде

	DbfWriter.Save("path.dbf", data);
	где data - List<PATHDBF>