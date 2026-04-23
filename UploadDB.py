import psycopg2

DB_HOST = "DB_HOST"
DB_PORT = 50632
DB_NAME = "DB_NAME"
DB_USER = "DB_USER"
DB_PASSWORD = "DB_PASSWORD"

SQL_FILE = "Untitled.sql"

def reset_and_load_sql(filename):
    conn = psycopg2.connect(
        host=DB_HOST,
        port=DB_PORT,
        dbname=DB_NAME,
        user=DB_USER,
        password=DB_PASSWORD
    )

    try:
        with conn:
            with conn.cursor() as cur:
                cur.execute("""
                    SELECT tablename
                    FROM pg_tables
                    WHERE schemaname = 'public';
                """)
                tables = [row[0] for row in cur.fetchall()]

                for table in tables:
                    cur.execute(f'DROP TABLE IF EXISTS "{table}" CASCADE;')
                print(f"Видалено таблиці: {tables}")

                with open(filename, "r", encoding="utf-8") as f:
                    sql_code = f.read()

                queries = [q.strip() for q in sql_code.split(";") if q.strip()]
                for q in queries:
                    cur.execute(q)

                print(f"SQL файл '{filename}' виконано успішно!")

    finally:
        conn.close()


if __name__ == "__main__":
    reset_and_load_sql(SQL_FILE)