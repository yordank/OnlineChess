namespace SampleChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
           
            
        }
        
        public override void Down()
        {
           
            DropTable("dbo.Messages");
        }
    }
}
