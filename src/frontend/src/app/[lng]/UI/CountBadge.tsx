'use client'
import {useAppSelector} from "@/hooks/redux";

const CountBadge = () => {
    const count = useAppSelector(x => x.productSlice.countForStore);
    return <span>{count}</span>;
};

export default CountBadge;