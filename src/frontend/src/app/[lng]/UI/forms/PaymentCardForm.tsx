"use client";
import React, { useState } from "react";
import { FiCreditCard } from "react-icons/fi";
import {paymentApi} from "@/services/paymentService";
import IAddPaymentModel from "@/models/payment/IAddPaymentModel";
import {FaCcVisa} from "react-icons/fa6";
import {RiMastercardLine} from "react-icons/ri";
import {useT} from "next-i18next/client";

interface FormErrors {
    cardHolder?: string;
    cardNumber?: string;
    month?: string;
    year?: string;
    cvc?: string;
}

export default function PaymentCardForm({closaModal}: {closaModal: ()=>void}) {
    const {t} = useT('payment_form');
    const [cardHolder, setCardHolder] = useState("");
    const [cardNumber, setCardNumber] = useState("");
    const [month, setMonth] = useState("");
    const [year, setYear] = useState("");
    const [cvc, setCvc] = useState("");
    const [errors, setErrors] = useState<FormErrors>({});
    const [submitted, setSubmitted] = useState(false);
    const [isFlipped, setIsFlipped] = useState(false);
    const [addCard] = paymentApi.useAddCardMutation();
    const formatCardNumber = (value: string) => {
        const digits = value.replace(/\D/g, "").slice(0, 16);
        return digits.replace(/(.{4})/g, "$1 ").trim();
    };

    const validate = (): FormErrors => {
        const next: FormErrors = {};
        if (!cardHolder.trim()) next.cardHolder = t('errors.cardHolderRequired');
        if (cardNumber.replace(/\s/g, "").length < 16)
            next.cardNumber = t('errors.cardNumberRequired');
        if (!month) next.month = t('errors.monthRequired');
        if (!year) next.year = t('errors.yearRequired');
        if (cvc.length < 3) next.cvc = t('errors.cvcRequired');
        return next;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setSubmitted(true);
        const next = validate();
        setErrors(next);
        const system = cardNumber.at(0) === "4" ? "Visa" : "MasterCard";
        if (Object.keys(next).length === 0) {
            console.log("Payment submitted", { cardHolder, cardNumber, month, year, cvc, system  });
            const val : IAddPaymentModel = {
                number: cardNumber,
                ownerName: cardHolder,
                cvv: cvc,
                paymentSystem: system,
                date: `${month}/${year}`
            }
            await addCard(val).unwrap();
            closaModal();
        }
    };

    const runValidation = (next: Partial<Record<keyof FormErrors, string>>) => {
        if (!submitted) return;
        setErrors((prev) => ({ ...prev, ...next }));
    };

    const digits = cardNumber.replace(/\s/g, "").padEnd(16, "•");
    const groups = [
        digits.slice(0, 4),
        digits.slice(4, 8),
        digits.slice(8, 12),
        digits.slice(12, 16),
    ];

    const months = Array.from({ length: 12 }, (_, i) => String(i + 1).padStart(2, "0"));
    const years = Array.from({ length: 12 }, (_, i) => String(new Date().getFullYear() + i));

    const inputStyle = (hasError?: string): React.CSSProperties => ({
        width: "100%",
        background: "var(--surface)",
        border: `1px solid ${hasError ? "var(--sale)" : "var(--border)"}`,
        borderRadius: "8px",
        padding: "12px 16px",
        color: "var(--text)",
        outline: "none",
        transition: "border-color 0.15s",
    });

    return (
        <div
            className="max-w-md mx-auto p-6 rounded-2xl"
            style={{ background: "var(--bg)" }}
        >
            {/* Card preview with flip */}
            <div
                className="mb-8 cursor-pointer select-none"
                style={{ perspective: "1200px" }}
                onClick={() => setIsFlipped((f) => !f)}
            >
                <div
                    className="relative w-full h-56 transition-transform duration-500"
                    style={{
                        transformStyle: "preserve-3d",
                        transform: isFlipped ? "rotateY(180deg)" : "rotateY(0deg)",
                    }}
                >
                    {/* Front */}
                    <div
                        className="absolute inset-0 rounded-2xl p-6"
                        style={{
                            backfaceVisibility: "hidden",
                            background: "var(--card)",
                            border: "1px solid var(--border)",
                        }}
                    >

                        <div
                            className="w-12 h-9 rounded-md mb-8 flex justify-center items-center "
                        >
                            {cardNumber === "" ?
                                <div
                                    className="w-full h-full rounded-md"
                                    style={{ background: "var(--accent)" }}
                                >

                                </div>
                                :
                                cardNumber.at(0) === "4" ? <FaCcVisa size={46} color='var(--accent)' /> : <RiMastercardLine size={46} color='var(--accent)' />
                            }
                        </div>

                        <div
                            className="flex gap-4 mb-8 font-mono text-lg tracking-widest"
                            style={{ color: "var(--text)" }}
                        >
                            {groups.map((group, i) => (
                                <span key={i} className="flex gap-1.5">
                                    {group.split("").map((char, j) =>
                                        char === "•" ? (
                                            <span
                                                key={j}
                                                className="w-2 h-2 rounded-full inline-block self-center"
                                                style={{ background: "var(--text)" }}
                                            />
                                        ) : (
                                            <span key={j}>{char}</span>
                                        )
                                    )}
                                </span>
                            ))}
                        </div>

                        <div className="flex items-end justify-between">
                            <div>
                                <p
                                    className="text-xs tracking-wider mb-1"
                                    style={{ color: "var(--muted)" }}
                                >
                                    {t('cardHolderLabel')}
                                </p>
                                <p
                                    className="font-semibold uppercase"
                                    style={{ color: "var(--text)" }}
                                >
                                    {cardHolder || t('yourNamePlaceholder')}
                                </p>
                            </div>
                            <div>
                                <p
                                    className="text-xs tracking-wider mb-1"
                                    style={{ color: "var(--muted)" }}
                                >
                                    {t('expiresLabel')}
                                </p>
                                <p className="font-semibold" style={{ color: "var(--text)" }}>
                                    {month || "MM"}/{year ? year.slice(-2) : "YY"}
                                </p>
                            </div>
                            <FiCreditCard
                                style={{ color: "var(--muted)" }}
                                size={28}
                                strokeWidth={1.5}
                            />
                        </div>
                    </div>

                    {/* Back */}
                    <div
                        className="absolute inset-0 rounded-2xl overflow-hidden"
                        style={{
                            backfaceVisibility: "hidden",
                            transform: "rotateY(180deg)",
                            background: "var(--card)",
                            border: "1px solid var(--border)",
                        }}
                    >
                        <div
                            className="w-full h-11 mt-6"
                            style={{ background: "var(--accent-soft)" }}
                        />

                        <div className="px-6 mt-6">
                            <p
                                className="text-xs tracking-wider mb-1"
                                style={{ color: "var(--muted)" }}
                            >
                                {t('cvcLabel')}
                            </p>
                            <div
                                className="rounded-md h-10 flex items-center justify-end px-4"
                                style={{ background: "var(--accent-soft)" }}
                            >
                                <span
                                    className="font-mono tracking-widest"
                                    style={{ color: "var(--text)" }}
                                >
                                    {cvc.padEnd(3, "•")}
                                </span>
                            </div>
                        </div>


                    </div>
                </div>
            </div>

            {/* Form */}
            <form onSubmit={handleSubmit} className="space-y-5">
                <div>
                    <label className="block mb-2" style={{ color: "var(--text)" }}>
                        {t('cardholderName')}
                    </label>
                    <input
                        type="text"
                        placeholder="John Doe"
                        value={cardHolder}
                        onChange={(e) => {
                            setCardHolder(e.target.value);
                            runValidation({
                                cardHolder: e.target.value.trim() ? undefined : t('errors.cardHolderRequired'),
                            });
                        }}
                        style={inputStyle(errors.cardHolder)}
                    />
                    {errors.cardHolder && (
                        <p className="text-sm mt-1" style={{ color: "var(--sale)" }}>
                            {errors.cardHolder}
                        </p>
                    )}
                </div>

                <div>
                    <label className="block mb-2" style={{ color: "var(--text)" }}>
                        {t('cardNumber')}
                    </label>
                    <input
                        type="text"
                        inputMode="numeric"
                        placeholder="1234 5678 9012 3456"
                        value={cardNumber}
                        onChange={(e) => {
                            const formatted = formatCardNumber(e.target.value);
                            setCardNumber(formatted);
                            runValidation({
                                cardNumber:
                                    formatted.replace(/\s/g, "").length < 16
                                        ? t('errors.cardNumberRequired')
                                        : undefined,
                            });
                        }}
                        className="font-mono"
                        style={inputStyle(errors.cardNumber)}
                    />
                    {errors.cardNumber && (
                        <p className="text-sm mt-1" style={{ color: "var(--sale)" }}>
                            {errors.cardNumber}
                        </p>
                    )}
                </div>

                <div className="grid grid-cols-3 gap-4">
                    <div>
                        <label className="block mb-2" style={{ color: "var(--text)" }}>
                            {t('month')}
                        </label>
                        <select
                            value={month}
                            onChange={(e) => {
                                setMonth(e.target.value);
                                runValidation({ month: e.target.value ? undefined : t('errors.monthRequired') });
                            }}
                            className="appearance-none"
                            style={inputStyle(errors.month)}
                        >
                            <option value="" style={{ background: "var(--card)" }}>MM</option>
                            {months.map((m) => (
                                <option key={m} value={m} style={{ background: "var(--card)" }}>
                                    {m}
                                </option>
                            ))}
                        </select>
                        {errors.month && (
                            <p className="text-sm mt-1" style={{ color: "var(--sale)" }}>
                                {errors.month}
                            </p>
                        )}
                    </div>

                    <div>
                        <label className="block mb-2" style={{ color: "var(--text)" }}>
                            {t('year')}
                        </label>
                        <select
                            value={year}
                            onChange={(e) => {
                                setYear(e.target.value);
                                runValidation({ year: e.target.value ? undefined : t('errors.yearRequired') });
                            }}
                            className="appearance-none"
                            style={inputStyle(errors.year)}
                        >
                            <option value="" style={{ background: "var(--card)" }}>YYYY</option>
                            {years.map((y) => (
                                <option key={y} value={y} style={{ background: "var(--card)" }}>
                                    {y}
                                </option>
                            ))}
                        </select>
                        {errors.year && (
                            <p className="text-sm mt-1" style={{ color: "var(--sale)" }}>
                                {errors.year}
                            </p>
                        )}
                    </div>

                    <div>
                        <label className="block mb-2" style={{ color: "var(--text)" }}>
                            {t('cvc')}
                        </label>
                        <input
                            type="text"
                            inputMode="numeric"
                            placeholder="123"
                            maxLength={3}
                            value={cvc}
                            onFocus={() => setIsFlipped(true)}
                            onBlur={() => setIsFlipped(false)}
                            onChange={(e) => {
                                const v = e.target.value.replace(/\D/g, "").slice(0, 4);
                                setCvc(v);
                                runValidation({ cvc: v.length < 3 ? t('errors.cvcRequired') : undefined });
                            }}
                            style={inputStyle(errors.cvc)}
                        />
                        {errors.cvc && (
                            <p className="text-sm mt-1" style={{ color: "var(--sale)" }}>
                                {errors.cvc}
                            </p>
                        )}
                    </div>
                </div>

                <button
                    type="submit"
                    className="w-full font-medium py-3 rounded-lg mt-4 transition-opacity hover:opacity-90"
                    style={{ background: "var(--btn)", color: "#ffffff" }}
                >
                    {t('addButton')}
                </button>
            </form>
        </div>
    );
}