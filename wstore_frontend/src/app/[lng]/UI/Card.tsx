'use client'
import React, {useState} from 'react';
import {FiCreditCard} from "react-icons/fi";
import IPaymentModel from "@/models/payment/IPaymentModel";
import {FaCcVisa} from "react-icons/fa6";
import {RiMastercardLine} from "react-icons/ri";
import Icon from "@/app/[lng]/UI/Icon";
import {MdDelete, MdEdit} from "react-icons/md";
import UseModal from "@/hooks/useModal";
import EditPaymentCardForm from "@/app/[lng]/UI/forms/EditPaymentCardForm";
import Modal from "@/app/[lng]/UI/Modal";
import {paymentApi} from "@/services/paymentService";
import {useT} from "next-i18next/client";

const Card = ({card} : {card : IPaymentModel}) => {
    const {t} = useT('card');
    const [isFlipped_, setIsFlipped_] = useState(false);
    const digits = card.number.replace(/\s/g, "").padEnd(16, "•");
    const groups = [
        digits.slice(0, 4),
        digits.slice(4, 8),
        digits.slice(8, 12),
        digits.slice(12, 16),
    ];
    const [deleteCard] = paymentApi.useDeleteCardMutation();
    const {isOpen, openModal, closeModal} = UseModal();
    const date = card.date.split('/');
    // console.log(card);
    return (
        <>
        <div className="flex justify-center">
            <div
                className="mb-8 cursor-pointer select-none w-6/10 relative"
                style={{ perspective: "1200px" }}
                onClick={() => setIsFlipped_((f) => !f)}
            >
                <div
                    className="relative w-full h-56 transition-transform duration-500"
                    style={{
                        transformStyle: "preserve-3d",
                        transform: isFlipped_ ? "rotateY(180deg)" : "rotateY(0deg)",
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
                            {card.paymentSystem === "Visa" ? <FaCcVisa size={46} color='var(--accent)' /> : <RiMastercardLine size={46} color='var(--accent)' />
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
                                    CARD HOLDER
                                </p>
                                <p
                                    className="font-semibold uppercase"
                                    style={{ color: "var(--text)" }}
                                >
                                    {card.ownerName || "YOUR NAME"}
                                </p>
                            </div>
                            <div>
                                <p
                                    className="text-xs tracking-wider mb-1"
                                    style={{ color: "var(--muted)" }}
                                >
                                    EXPIRES
                                </p>
                                <p className="font-semibold" style={{ color: "var(--text)" }}>
                                    {date[0] || "MM"}/{date[1] ? date[1].slice(-2) : "YY"}
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
                                CVC
                            </p>
                            <div
                                className="rounded-md h-10 flex items-center justify-end px-4"
                                style={{ background: "var(--accent-soft)" }}
                            >
                                <span
                                    className="font-mono tracking-widest"
                                    style={{ color: "var(--text)" }}
                                >
                                    {card.cvv.padEnd(3, "•")}
                                </span>
                            </div>
                        </div>


                    </div>
                </div>
                {!isFlipped_ &&
                    <div>
                        <div className="absolute top-2 right-2">
                            <Icon width={40} height={40}>
                                <MdEdit size={22} color="var(--accent-mid)" onClick={(e)=>{
                                    e.stopPropagation()
                                    openModal()
                                }} />
                            </Icon>
                        </div>
                        <div className="absolute top-2 right-14">
                            <Icon width={40} height={40}>
                                <MdDelete size={22} color="var(--accent-mid)" onClick={() => deleteCard(card.id)} />

                            </Icon>
                        </div>
                    </div>
                }

            </div>



        </div>
            <Modal isOpen={isOpen} closeModal={closeModal} size='md' title={t('edit')}>
                <EditPaymentCardForm closaModal={closeModal} card={card} />
            </Modal>
        </>
    );
};

export default Card;