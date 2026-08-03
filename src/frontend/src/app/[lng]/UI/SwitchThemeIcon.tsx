"use client";

import { useTheme } from "next-themes";
import { useState, useEffect } from "react";
import Icon from "@/app/[lng]/UI/Icon";
import { FaMoon, FaSun } from "react-icons/fa";

export default function SwitchThemeIcon() {
    const { resolvedTheme, setTheme } = useTheme();
    const [mounted, setMounted] = useState(false);

    useEffect(() => setMounted(true), []);

    if (!mounted) {
        return (
            <Icon height={40} width={40}>
                <div style={{ width: 21, height: 21 }} />
            </Icon>
        );
    }

    const switchTheme = () =>
        setTheme(resolvedTheme === "dark" ? "light" : "dark");

    return (
        <div onClick={switchTheme}>
            <Icon height={40} width={40}>
                {resolvedTheme === "dark"
                    ? <FaMoon size={21} color="var(--accent-mid)" />
                    : <FaSun size={21} color="var(--accent-mid)" />}
            </Icon>
        </div>
    );
}