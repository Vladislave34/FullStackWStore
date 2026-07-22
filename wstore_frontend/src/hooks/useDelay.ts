import {useEffect, useState} from "react";


export function useDelay<T>(value: T, delay: number): T {
    const [delayVal, setDelayVal] = useState(value)
    useEffect(()=>{
        const  timer = setTimeout(()=>{
            setDelayVal(value)
        }, delay)
        return () => clearTimeout(timer);
    }, [delay, value])
    return delayVal
}