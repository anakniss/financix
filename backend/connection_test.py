from sqlalchemy import create_engine

connection_string = 'mysql+pymysql://root:root@localhost:3306/financix'
engine = create_engine(connection_string)

try:
    with engine.connect() as connection:
        print("Conexão bem-sucedida!")
except Exception as e:
    print(f"Erro ao conectar: {e}")
