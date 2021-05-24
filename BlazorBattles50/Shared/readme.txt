

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