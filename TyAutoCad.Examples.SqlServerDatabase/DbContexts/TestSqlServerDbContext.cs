using System;
using System.Data.Entity;
using System.Linq;
using TyAutoCad.Examples.SqlServerDatabase.Entities;

namespace TyAutoCad.Examples.SqlServerDatabase.DbContexts
{
    public class TestSqlServerDbContext : DbContext
    {
        // コンテキストは、アプリケーションの構成ファイル (App.config または Web.config) から 'TestSqlServerDbContext' 
        // 接続文字列を使用するように構成されています。既定では、この接続文字列は LocalDb インスタンス上
        // の 'TyAutoCad.Examples.SqlServerDatabase.DbContexts.TestSqlServerDbContext' データベースを対象としています。 
        // 
        // 別のデータベースとデータベース プロバイダーまたはそのいずれかを対象とする場合は、
        // アプリケーション構成ファイルで 'TestSqlServerDbContext' 接続文字列を変更してください。
        public TestSqlServerDbContext()
            : base("name=TestSqlServerDbContext")
        {
        }

        public TestSqlServerDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }

        // モデルに含めるエンティティ型ごとに DbSet を追加します。Code First モデルの構成および使用の
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=390109 を参照してください。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public DbSet<TestLine> TestLines { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}