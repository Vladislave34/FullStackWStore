import Link from "next/link";
import {FaInstagram, FaFacebookF, FaTelegramPlane} from "react-icons/fa";

interface FooterProps {
    lng?: string;
}

const Footer = ({ lng = "uk" }: FooterProps) => {
    const columns = [
        {
            title: "Магазин",
            links: [
                { label: "Каталог", href: `/${lng}/products` },
                { label: "Новинки", href: `/${lng}/products?sort=new` },
                { label: "Розпродаж", href: `/${lng}/products?sale=true` },
            ],
        },
        {
            title: "Інформація",
            links: [
                { label: "Про нас", href: `/${lng}/about` },
                { label: "Доставка та оплата", href: `/${lng}/delivery` },
                { label: "Повернення", href: `/${lng}/returns` },
            ],
        },
        {
            title: "Підтримка",
            links: [
                { label: "Контакти", href: `/${lng}/contacts` },
                { label: "FAQ", href: `/${lng}/faq` },
            ],
        },
    ];

    return (
        <footer
            style={{ background: "var(--surface)", borderTop: "1px solid var(--border)" }}
            className="w-full z-20 "
        >
            <div className="max-w-7xl mx-auto px-6 py-12 grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-10">
                <div className="flex flex-col gap-3">
                    <span style={{ color: "var(--text)" }} className="text-xl font-semibold tracking-tight">
                        WStore
                    </span>
                    <p style={{ color: "var(--muted)" }} className="text-sm leading-relaxed max-w-xs">
                        Речі, які поєднують якість і стиль. Обирайте те, що вам до вподоби.
                    </p>
                    <div className="flex gap-3 mt-2">
                        <a
                            href="#"
                            style={{ background: "var(--accent-soft)", color: "var(--accent)" }}
                            className="w-9 h-9 flex items-center justify-center rounded-full hover:opacity-80 transition-opacity"
                        >
                            <FaInstagram size={16} />
                        </a>
                        <a
                            href="#"
                            style={{ background: "var(--accent-soft)", color: "var(--accent)" }}
                            className="w-9 h-9 flex items-center justify-center rounded-full hover:opacity-80 transition-opacity"
                        >
                            <FaFacebookF size={16} />
                        </a>
                        <a
                            href="#"
                            style={{ background: "var(--accent-soft)", color: "var(--accent)" }}
                            className="w-9 h-9 flex items-center justify-center rounded-full hover:opacity-80 transition-opacity"
                        >
                            <FaTelegramPlane size={16} />
                        </a>
                    </div>
                </div>

                {columns.map((col) => (
                    <div key={col.title} className="flex flex-col gap-3">
                        <span style={{ color: "var(--text)" }} className="text-sm font-semibold uppercase tracking-wide">
                            {col.title}
                        </span>
                        <div className="flex flex-col gap-2">
                            {col.links.map((link) => (
                                <Link
                                    key={link.href}
                                    href={link.href}
                                    style={{ color: "var(--muted)" }}
                                    className="text-sm hover:opacity-80 transition-opacity w-fit"
                                >
                                    {link.label}
                                </Link>
                            ))}
                        </div>
                    </div>
                ))}
            </div>

            <div
                style={{ borderTop: "1px solid var(--border)" }}
                className="max-w-7xl mx-auto px-6 py-5 flex flex-col sm:flex-row justify-between items-center gap-2"
            >
                <span style={{ color: "var(--muted)" }} className="text-xs">
                    © {new Date().getFullYear()} WStore. Усі права захищені.
                </span>
                <div className="flex gap-4">
                    <Link href={`/${lng}/privacy`} style={{ color: "var(--muted)" }} className="text-xs hover:opacity-80">
                        Політика конфіденційності
                    </Link>
                    <Link href={`/${lng}/terms`} style={{ color: "var(--muted)" }} className="text-xs hover:opacity-80">
                        Умови використання
                    </Link>
                </div>
            </div>
        </footer>
    );
};

export default Footer;