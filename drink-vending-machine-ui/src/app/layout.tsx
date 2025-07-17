import type {Metadata} from "next";
import {Geist, Geist_Mono} from "next/font/google";
import "./globals.css";
import React from "react";
import {VendingLockGuard} from "@/components/ui/vending-lock-guard";
import {OrderProvider} from "@/context/order-context";

const geistSans = Geist({
    variable: "--font-geist-sans",
    subsets: ["latin"],
});

const geistMono = Geist_Mono({
    variable: "--font-geist-mono",
    subsets: ["latin"],
});

export const metadata: Metadata = {
    title: "Vending Machine",
    description: "Автомат с напитками",
};

export default function RootLayout({
                                       children,
                                   }: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html lang="en">
        <body
            className={`${geistSans.variable} ${geistMono.variable} antialiased`}
        >
        <VendingLockGuard>
            <OrderProvider>
                {children}
            </OrderProvider>
        </VendingLockGuard>
        </body>
        </html>
    );
}
