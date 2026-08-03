
import {FC} from "react";
import {usePathname, useRouter, useSearchParams} from "next/navigation";

export const PaginationItem: FC<{num: number, currentPage:number}> = ({ num, currentPage }) => {

    const router = useRouter();
    const pathname = usePathname();
    const searchParams = useSearchParams();
    const isActive = currentPage === num;
    const handleClick = () => {
        const params = new URLSearchParams(searchParams.toString());
        params.set("page", String(num));
        router.push(`${pathname}?${params.toString()}`);
    };
    return (
        <div
            className={`
                text-base cursor-pointer font-semibold
                border-2 rounded-xl h-10 w-10 flex justify-center items-center
                transition-colors
                ${isActive
                ? "bg-[var(--btn)] text-[var(--bg)] border-[var(--btn)]"
                : "bg-[var(--card)] text-[var(--accent-mid)] border-[var(--border)] hover:bg-[var(--accent-soft)]"
            }
            `}
            onClick={handleClick}
        >
            {num}
        </div>
    );
};