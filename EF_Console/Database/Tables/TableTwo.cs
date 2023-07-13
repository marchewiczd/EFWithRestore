using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Console.Database.Tables
{
    [Table("TABLE_TWO")]
    public class TableTwo : TableBase
    {
        [Key]
        [Column("key_id")]
        public string Id { get; set; }

        [Column("descr")]
        public string Description { get; set; }

        
    }
}

/*
    CREATE TABLE TABLE_TWO (
        key_id varchar(4) not null,
        descr varchar(200) not null,
        PRIMARY KEY(key_id)
    );

    INSERT INTO TABLE_TWO (key_id, descr)
    VALUES ('BA','very long and meaningless description');
    INSERT INTO TABLE_TWO (key_id, descr)
    VALUES ('BB','lorem ipsum blablabla');

*/