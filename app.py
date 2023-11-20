from flask import Flask, request, jsonify
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///leaderboard.db'
db = SQLAlchemy(app)  

class Score(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    player_name = db.Column(db.String(255), nullable=True)  # Adjust the length as needed
    score = db.Column(db.Integer, nullable=False)

@app.route('/leaderboard', methods=['GET'])
def get_leaderboard():
    leaderboard = Score.query.order_by(Score.score.desc()).limit(10).all()
    leaderboard_data = [{'score': entry.score} for entry in leaderboard]
    return jsonify(leaderboard_data)

@app.route('/leaderboard', methods=['POST'])
def add_score():
    data = request.get_json()
    score = data.get('score')
    player_name = data.get('player_name', None)  # Default to None if not provided

    if not score:
        return jsonify({'error': 'Invalid data'}), 400

    new_score = Score(player_name=player_name, score=score)
    db.session.add(new_score)
    db.session.commit()

    return jsonify({'message': 'Score added successfully'}), 201

if __name__ == '__main__':
    with app.app_context():
        db.create_all()
    app.run(debug=True)
