interface Payment {
    id: string;
    number: string;
    date: string;
    cvv: string;
    ownerName: string;
    paymentSystem: string;
}

interface OrderItem {
    id: string;
    productVariantId: string;
    productName: string;
    productNameUk: string;
    colorName: string;
    colorNameUk: string;
    sizeName: string;
    quantity: number;
    price: number;
    images: string[];
}

export type OrderStatus = 'Pending' | "Confirmed" | 'Processing' | 'Shipped' | 'Delivered' | 'Cancelled' | "Refunded" | "Failed";

export default interface Order {
    id: string;
    totalPrice: number;
    orderStatus: OrderStatus;
    userId: string;
    createdAt: string;
    address: string;
    payment: Payment;
    items: OrderItem[];
}