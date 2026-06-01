"use client";

import { useTheme } from "next-themes";

import Image from "next/image";
import Icon from "@/UI/Icon";
import {FaMoon, FaSun} from "react-icons/fa";

export default function SwitchThemeIcon() {
    const { resolvedTheme, setTheme } = useTheme();

    if (!resolvedTheme) {
        return (
            <Icon height={40} width={40}>
                <div style={{ width: 24, height: 24 }} />
            </Icon>
        );
    }

    const switchTheme = () =>
        setTheme(resolvedTheme === "dark" ? "light" : "dark");



    return (
        <div onClick={switchTheme}>
            <Icon height={40} width={40}>
                {/*<Image*/}
                {/*    src={resolvedTheme === "dark" ? "/moon.png" : "/ocean.png"}*/}
                {/*    alt="Theme"*/}
                {/*    width={24}*/}
                {/*    height={24}*/}
                {/*    priority*/}
                {/*/>*/}
                {resolvedTheme === "dark" ? <FaMoon size={21} color="var(--accent-mid)" /> : <FaSun size={21} color="var(--accent-mid)" /> }
            </Icon>
        </div>
    );
}