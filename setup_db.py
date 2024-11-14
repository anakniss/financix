from website import create_app
from website.database import db

app = create_app()
with app.app_context():
    db.create_all()
    print("Tabelas criadas com sucesso!")
