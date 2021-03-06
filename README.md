# Prototypes.API.PaymentGateway

This is a prototype API that aims to build the basis of a payment gateway. Please see the documentation below for how the initial prototype is working.

# Documentation

## Authentication
The Web API uses Basic Authentication that would allow a merchant to have a unique key that is required to interact with the gateway endpoints. The application currently only uses one static API key, but this will be built upon in future for multiple merchants.

In order to use the API, please add the "Authorization" header below:

``Authorization: Basic dGVzdDp0ZXN0``

## Payments API - Debit request
### POST: /payment/debit
To make a debit request you must supply a Payment object. A payment object is made up of the following fields:

Please note: not all field requirements have been established yet. but some basic conditions are as below.

| Field               | Type      | Required         | Conditions                                                | Example                       |
| ------------------- |-----------| ---------------- | --------------------------------------------------------- | ------------------------------|
| cardType            | String    | true             | Visa, MasterCard or AmericanExpress                       | "Visa"                        |
| cardNumber          | String    | true             | Max 16 chars long                                         | "4111111111111111"            |
| cardExpiry          | String    | true             | MM/yy format                                              | "01/25"                       |
| cardCvv             | String    | true             | 3-4 chars long                                            | "123"                         |
| amount              | Number    | true             | Greater than 0, less than equal to 9999                   | 99.99                         | 
| currency            | String    | true             | Should be an ISO currency code and atleast 3 chars long   | "GBP"                         |

### Example request
``POST: https://localhost:44320/Payment/debit``
```
{
  "cardType": "Visa",
  "cardNumber": "4111111111111111",
  "cardCvv": "123",
  "cardExpiry": "01/25",
  "amount": 9.99,
  "currency": "GBP"
}
```

### Responses

| Payment Status      | Http Status Code | Response body                          |
| ------------------- | ---------------- | -------------------------------------- |
| Successful          | 200              | See example below                      |
| Rejected            | 400              | See example below                      |


### Example successful response
```
{
    "id": "-N6AL8u3bkZmkUP1pv4y",
    "isSuccess": true,
    "bankResponseCode": "TEST12345",
    "message": "Accepted",
    "dateCreated": "2022-07-04T23:33:11.4098676+01:00",
    "payment": {
        "cardType": "Visa",
        "cardNumber": "************1111",
        "cardExpiry": "01/25",
        "cardCvv": "***",
        "amount": 9.99,
        "currency": "GBP"
    }
}

```
### Example unsuccessful response
```
{
    "transactionId": null,
    "isSuccess": false,
    "message": "Invalid Request",
    "errors": [
        "Card type is invalid",
        "Card number must be 16 digits long",
        "'Card Cvv' must be between 3 and 4 characters. You entered 2 characters."
    ],
    "payment": null
}
```

## Payments API - Get Payment
### GET: /payment/{id}
To retrieve a payment you must supply the transactionId from the payment.

### Example request
``GET: https://localhost:44320/Payment/-N6AL8u3bkZmkUP1pv4y``

### Responses

| Payment Status      | Http Status Code | Response body                          |
| ------------------- | ---------------- | -------------------------------------- |
| Successful          | 200              | See example below                      |
| Rejected            | 400              | "Record not found"                     |

### Example successful response
```
{
    "id": "-N6AL8u3bkZmkUP1pv4y",
    "isSuccess": true,
    "bankResponseCode": "TEST12345",
    "message": "Accepted",
    "dateCreated": "2022-07-04T23:33:11.4098676+01:00",
    "payment": {
        "cardType": "Visa",
        "cardNumber": "************1111",
        "cardExpiry": "01/25",
        "cardCvv": "***",
        "amount": 9.99,
        "currency": "GBP"
    }
}
```
# Tools
## Postman
My Postman collection can be found [here](https://github.com/mhayward-dev/Prototypes.API.PaymentGateway/blob/main/Tools/ProtoType.API.PaymentGateway.postman_collection.json) in the project for use.


# Notes
* I am using Azure Table Storage in the solution as the database. A flat record structure is not ideal for storing inter-related entities so this could be changed to a Relational Database in future, but right now I am using a NOSQL database purely to avoid hosting costs.
* The Visa card type is the only functioning bank at this moment in time. Mastercard and Amex will throw an exception.
