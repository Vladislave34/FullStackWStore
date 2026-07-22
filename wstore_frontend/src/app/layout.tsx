import './globals.css'
import { Inter } from "next/font/google";
import type { ReactNode } from 'react'

const inter = Inter({
    subsets: ["latin", "cyrillic"],
    weight: ["400", "500"],
});

export default function RootLayout({ children }: { children: ReactNode }) {
    return (
        <html suppressHydrationWarning>
        <head>
            <script
                dangerouslySetInnerHTML={{
                    __html: `
                          (function() {
                            try {
                              var theme = localStorage.getItem('theme');
                              if (theme === 'dark' || (!theme && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
                                document.documentElement.classList.add('dark');
                              }
                            } catch(e) {}
                          })();
                        `,
                }}
            />
        </head>
        <body style={{ background: "var(--bg)" }} className={inter.className}>
        {children}
        </body>
        </html>
    )
}