import {memo} from "react";
import Image from "next/image";
import Icon from "@/UI/Icon";
import SaleLabel from "@/UI/SaleLabel";
import LikeLabel from "@/UI/LikeLabel";


const ProductCard = () => {
    return (
        <div style={{
            background: "var(--card)"
        }}
            className="rounded-xl w-[calc(25%-12px)] overflow-hidden relative">
            <div className="flex justify-center items-center rounded-t-xl">
            <Image src="https://picsum.photos/seed/summer-dress/600/400" width={600} height={400} alt={""} />
            </div>
            <div className="flex flex-row justify-between p-4">
                <div className="flex flex-col">
                    <p
                        style={{
                            color: "var(--text)"
                        }}
                    >Літнє плаття</p>

                    <p
                        style={{
                            color: "var(--muted)"
                        }}
                    >Zara</p>
                    <p
                        style={{
                            color: "var(--price)"
                        }}
                        className="mt-2"
                    >499₴</p>
                </div>
                <div className="flex items-end">
                    <Icon width={32} height={32}>
                        +
                    </Icon>
                </div>
            </div>
            <SaleLabel />
            <LikeLabel />
        </div>
    );
};

export default memo(ProductCard);