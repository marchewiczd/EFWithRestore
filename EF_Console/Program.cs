using EF_Console.Database;
using EF_Console.Database.Tables;

using var db = new ToolContext();

var currReg = db.Find<TableOne>("AA");
currReg.Description = "new and very much worse description";

db.Update(currReg);
db.Restore<TableOne>();