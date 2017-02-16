namespace app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimagetovideos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "VideoImg", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "VideoImg");
        }
    }
}
