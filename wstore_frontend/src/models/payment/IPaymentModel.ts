export default interface IPaymentModel {
    id: string;
    number: string;
    date: string;

    cvv: string;
    ownerName: string;
    paymentSystem: string;
}