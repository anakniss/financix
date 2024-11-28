from datetime import datetime
from .database import db 

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    email = db.Column(db.String(150), unique=True, nullable=False)
    password = db.Column(db.String(150), nullable=False)
    first_name = db.Column(db.String(150))

class Transaction(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    tipo = db.Column(db.String(10))
    valor = db.Column(db.Float, nullable=False)
    categoria = db.Column(db.String(50))
    data = db.Column(db.Date, default=datetime.utcnow)
    descricao = db.Column(db.String(255))
