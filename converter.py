from flask import Flask, request, jsonify
from forex_python.converter import CurrencyRates

app = Flask(__name__)

@app.route('/convert', methods=['POST'])
def convert_currency():
    data = request.json
    currency_rates = CurrencyRates()
    amount = data['amount']
    from_currency = data['from_currency']
    to_currency = data['to_currency']
    try:
        converted_amount = currency_rates.convert(from_currency, to_currency, amount)
        response = {"converted_amount": converted_amount}
    except Exception as e:
        response = {"error": str(e)}
    return jsonify(response)

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)
