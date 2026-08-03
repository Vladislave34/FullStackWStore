import {useState} from "react";
import Order, {OrderStatus} from "@/models/order/IOrderModel";
import {useT} from "next-i18next/client";


// const statusMap: Record<OrderStatus, { label: string; dot: string }> = {
//     Pending: { label: "Очікує", dot: "#B4B2A9" },
//     Confirmed: { label: "Підтверджено", dot: "#378ADD" },
//     Processing: { label: "В обробці", dot: "#0C447C" },
//     Shipped: { label: "Відправлено", dot: "#854F0B" },
//     Delivered: { label: "Доставлено", dot: "#0F6E56" },
//     Cancelled: { label: "Скасовано", dot: "#E24B4A" },
//     Refunded: { label: "Повернено", dot: "#993C1D" },
//     Failed: { label: "Помилка", dot: "#791F1F" },
// };

function formatDate(iso: string) {
    return new Date(iso).toLocaleDateString("uk-UA", {
        day: "2-digit",
        month: "short",
        year: "numeric",
    });
}

function formatPrice(value: number) {
    return new Intl.NumberFormat("uk-UA").format(value) + " ₴";
}

export function OrderRow({ order, lng }: { order: Order, lng: string }) {
    const [open, setOpen] = useState(false);
    const {t} = useT('order_status')
    const statusMap: Record<OrderStatus, { label: string; dot: string }> = {
        Pending: { label: t('Pending'), dot: "#B4B2A9" },
        Confirmed: { label: t('Confirmed'), dot: "#378ADD" },
        Processing: { label: t('Processing'), dot: "#0C447C" },
        Shipped: { label: t('Shipped'), dot: "#854F0B" },
        Delivered: { label: t('Delivered'), dot: "#0F6E56" },
        Cancelled: { label: t('Cancelled'), dot: "#E24B4A" },
        Refunded: { label: t('Refunded'), dot: "#993C1D" },
        Failed: { label: t('Failed'), dot: "#791F1F" },
    };
    const status = statusMap[order.orderStatus];
    const firstItem = order.items[0];
    const extraCount = order.items.length - 1;


    return (
        <div
            className="rounded-2xl border transition-colors w-full flex-shrink-0"
            style={{ background: "var(--card)", borderColor: "var(--border)" }}
        >
            <button
                onClick={() => setOpen((o) => !o)}
                className="w-full flex items-center gap-4 p-4 text-left"
            >
                {/* thumbnail */}
                <div
                    className="shrink-0 w-16 h-16 rounded-xl overflow-hidden border"
                    style={{ borderColor: "var(--border)", background: "var(--accent-soft)" }}
                >
                    {firstItem?.images?.[0] && (
                        <img
                            src={firstItem.images[0]}
                            alt={firstItem.productName}
                            className="w-full h-full object-cover"
                        />
                    )}
                </div>

                {/* main info */}
                <div className="min-w-0 flex-1">
                    <div className="flex items-center gap-2 flex-wrap">
            <span
                className="text-sm font-medium truncate"
                style={{ color: "var(--text)" }}
            >
              {lng ==="en" ? firstItem?.productName : firstItem?.productNameUk}
            </span>
                        {extraCount > 0 && (
                            <span
                                className="text-xs px-2 py-0.5 rounded-full"
                                style={{ background: "var(--tag)", color: "var(--tag-text)" }}
                            >
                +{extraCount}
              </span>
                        )}
                    </div>
                    <p className="text-xs mt-1 truncate" style={{ color: "var(--muted)" }}>
                        {formatDate(order.createdAt)} · {order.address}
                    </p>
                </div>

                {/* status */}
                <div className="hidden sm:flex items-center gap-2 shrink-0">
          <span
              className="w-2 h-2 rounded-full"
              style={{ background: status.dot }}
          />
                    <span className="text-xs" style={{ color: "var(--muted)" }}>
            {status.label}
          </span>
                </div>

                {/* price */}
                <div className="text-right shrink-0">
                    <p className="text-sm font-semibold" style={{ color: "var(--price)" }}>
                        {formatPrice(order.totalPrice)}
                    </p>
                    <p className="text-xs sm:hidden" style={{ color: "var(--muted)" }}>
                        {status.label}
                    </p>
                </div>

                <svg
                    className="shrink-0 transition-transform"
                    style={{
                        transform: open ? "rotate(180deg)" : "rotate(0deg)",
                        color: "var(--muted)",
                    }}
                    width="16"
                    height="16"
                    viewBox="0 0 24 24"
                    fill="none"
                >
                    <path
                        d="M6 9l6 6 6-6"
                        stroke="currentColor"
                        strokeWidth="2"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                    />
                </svg>
            </button>

            {open && (
                <div
                    className="border-t px-4 py-3 space-y-3"
                    style={{ borderColor: "var(--border)" }}
                >
                    {order.items.map((item) => (
                        <div key={item.id} className="flex items-center gap-3">
                            <div
                                className="shrink-0 w-12 h-12 rounded-lg overflow-hidden border"
                                style={{ borderColor: "var(--border)", background: "var(--accent-soft)" }}
                            >
                                {item.images?.[0] && (
                                    <img
                                        src={item.images[0]}
                                        alt={item.productNameUk}
                                        className="w-full h-full object-cover"
                                    />
                                )}
                            </div>
                            <div className="min-w-0 flex-1">
                                <p className="text-sm truncate" style={{ color: "var(--text)" }}>
                                    {item.productNameUk}
                                </p>
                                <p className="text-xs" style={{ color: "var(--muted)" }}>
                                    {item.colorNameUk}, {item.sizeName} · {item.quantity} шт.
                                </p>
                            </div>
                            <p className="text-sm shrink-0" style={{ color: "var(--text)" }}>
                                {formatPrice(item.price * item.quantity)}
                            </p>
                        </div>
                    ))}

                    <div
                        className="flex items-center justify-between pt-2 border-t text-xs"
                        style={{ borderColor: "var(--border)", color: "var(--muted)" }}
                    >
                        <span>№ {order.id.slice(0, 8)}</span>
                        <span>
              {order.payment.paymentSystem} •••• {order.payment.number.slice(-4)}
            </span>
                    </div>
                </div>
            )}
        </div>
    );
}

export default OrderRow;
