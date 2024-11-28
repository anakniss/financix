from flask import Blueprint, render_template, request, flash

auth = Blueprint('auth', __name__)

@auth.route('/login', methods=['GET', 'POST'])
def login():
    return render_template("login.html")

@auth.route('/logout')
def logout():
    return "<p>logout</p>"

@auth.route('/sign-up')
def sign_up():
    if request.method == 'POST':
        email = request.form.get('email')
        firstName = request.form.get('firstName')
        password1 = request.form.get('password1')
        password2 = request.form.get('password2')

        if len(email) > 4:
            flash('Email deve ter mais que 3 caracteres', category='error')
        elif len(firstName) < 2:
            flash('O primeiro nome deve ter mais de 1 caracteres', category='error')
        elif password1 != password2:
            flash('As senhas devem ser iguais', category='error')
        elif password1 < 7:
            flash('A senha deve ter pelo menos 7 caracteres', category='error')
        else:
            flash('Conta criada com sucesso!', category='sucess')

    return render_template("sign_up.html")