using System;
using System.Threading.Tasks;

#if __MonoCS__
namespace System.Data.SQLite
{
	public class SQLiteConnection {
		public Mono.Data.Sqlite.SqliteConnection __connection;

		public string ConnectionString {
			get { return __connection.ConnectionString; }
			set { __connection.ConnectionString = value; }
		}

		public SQLiteConnection () {
			__connection = new Mono.Data.Sqlite.SqliteConnection ();
		}

		public SQLiteConnection (string connectionString) {
			__connection = new Mono.Data.Sqlite.SqliteConnection (connectionString);
		}

		public SQLiteConnection (SQLiteConnection conn) {
			__connection = new Mono.Data.Sqlite.SqliteConnection (conn.__connection);
		}

		public void Open () {
			__connection.Open ();
		}

		public void Close () {
			__connection.Close ();
		}
	}

	public class SQLiteCommand {
		public Mono.Data.Sqlite.SqliteCommand __command;

		public string CommandText {
			get { return __command.CommandText; }
			set { __command.CommandText = value; }
		}

		public Mono.Data.Sqlite.SqliteParameterCollection Parameters {
			get { return __command.Parameters; }
		}

		public SQLiteCommand (SQLiteConnection connection) {
			__command = new Mono.Data.Sqlite.SqliteCommand (connection.__connection);
		}

		public SQLiteDataReader ExecuteReader () {
			return new SQLiteDataReader (__command.ExecuteReader ());
		}

		public void Dispose () {
			__command.Dispose ();
		}

		public int ExecuteNonQuery () {
			return __command.ExecuteNonQuery ();
		}

		public Task<int> ExecuteNonQueryAsync () {
			return __command.ExecuteNonQueryAsync ();
		}
	}

	public class SQLiteDataReader {
		public Mono.Data.Sqlite.SqliteDataReader __reader;

		public object this[string key] {
			get {
				return __reader [key];
			}
		}

		public SQLiteDataReader (Mono.Data.Sqlite.SqliteDataReader reader) {
			__reader = reader;
		}

		public bool Read() {
			return __reader.Read ();
		}

		public string GetString (int i) {
			return __reader.GetString(i);
		}
	}

	public class SQLiteParameter {
		public Mono.Data.Sqlite.SqliteParameter __parameter;

		public object Value {
			get { return __parameter.Value; }
			set { __parameter.Value = value; }
		}

		public SQLiteParameter (string str, System.Data.DbType type) {
			__parameter = new Mono.Data.Sqlite.SqliteParameter (str, type);
		}
	}
}
#endif
