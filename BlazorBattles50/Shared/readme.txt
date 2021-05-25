

// 安裝EF工具
dotnet tool install --global dotnet-ef

// 安裝EF套件
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Sqlite
Microsoft.EntityFrameworkCore.Tools


// 新增Migration
cd .\BlazorBattles50\Server
dotnet ef migrations add Initial

// 更新資料庫
dotnet ef database update

// 下戴DB Browser for SQLite
https://sqlitebrowser.org/

// API:Put更新資料庫
// 使用PostMan
https://localhost:5001/api/unit/1

    {
        "id": 1,
        "title": "騎士",
        "attack": 10,
        "defense": 10,
        "hitPoints": 100,
        "bananaCost": 100
    }