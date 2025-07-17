'use client';

import React, {createContext, useContext, useEffect, useMemo, useState} from 'react';
import {DrinkModel, DrinkService} from '@/app/api/drink-vending-machine';
import {Loading} from "@/components/ui/loading";

export interface OrderItem extends DrinkModel {
    quantitySelected: number;
}

interface OrderContextType {
    orderItems: OrderItem[];
    selectedDrinkIds: Set<number>;
    total: number;
    changeQuantity: (id: number, delta: number) => void;
    removeItem: (id: number) => void;
    addItem: (item: OrderItem[]) => void;
    syncWithSelectedIds: (ids: Set<number>) => void;
    isHydrated: boolean;
}

const OrderContext = createContext<OrderContextType | undefined>(undefined);

export const OrderProvider = ({children}: { children: React.ReactNode }) => {
    const [orderItems, setOrderItems] = useState<OrderItem[]>(() => {
        if (typeof window === 'undefined') return [];

        const stored = localStorage.getItem('selectedOrderItems');
        if (!stored) return [];
        try {
            return JSON.parse(stored);
        } catch {
            return [];
        }
    });

    const [total, setTotal] = useState(0);
    const [isHydrated, setIsHydrated] = useState(false);

    const syncWithSelectedIds = (ids: Set<number>) => {
        setOrderItems(prev => prev.filter(item => ids.has(item.id!)));
    };

    useEffect(() => {
        const loadFromStorage = async () => {
            try {
                const stored = localStorage.getItem('selectedOrderItems');
                if (!stored) return;

                const parsed: { id: number; quantitySelected: number }[] = JSON.parse(stored);
                const allDrinks = await DrinkService.getAllDrinks({});
                const selected = allDrinks.filter(d => parsed.some(p => p.id === d.id));

                const mapped = selected.map(d => {
                    const match = parsed.find(p => p.id === d.id)!;
                    return {
                        ...d,
                        quantitySelected: match.quantitySelected,
                    };
                });

                setOrderItems(mapped);
            } catch (e) {
                console.error('Error loading order items:', e);
            } finally {
                setIsHydrated(true);
            }
        };

        void loadFromStorage();
    }, []);

    useEffect(() => {
        const sum = orderItems.reduce((acc, item) => acc + (item.price || 0) * item.quantitySelected, 0);
        setTotal(sum);
    }, [orderItems]);

    useEffect(() => {
        if (!isHydrated) return;

        const plain = orderItems.map(item => ({
            id: item.id!,
            quantitySelected: item.quantitySelected,
        }));
        localStorage.setItem('selectedOrderItems', JSON.stringify(plain));
    }, [orderItems, isHydrated]);

    const changeQuantity = (id: number, delta: number) => {
        setOrderItems((prev) =>
            prev.map((item) => {
                if (item.id === id) {
                    const newQuantity = item.quantitySelected + delta;
                    if (newQuantity < 1 || newQuantity > (item.quantity || Infinity)) return item;
                    return {...item, quantitySelected: newQuantity};
                }
                return item;
            })
        );
    };

    const addItem = (items: OrderItem[]) => {
        setOrderItems(prev => {
            const existingIds = new Set(prev.map(item => item.id));
            const merged = [...prev];

            for (const newItem of items) {
                if (!existingIds.has(newItem.id)) {
                    merged.push(newItem);
                }
            }

            return merged;
        });
    };

    const removeItem = (id: number) => {
        setOrderItems((prev) => prev.filter((item) => item.id !== id));
    };

    const value = useMemo(() => {
        const selectedDrinkIds = new Set(
            orderItems
                .map(item => item.id)
                .filter((id): id is number => id !== undefined)
        );

        return {
            orderItems,
            total,
            changeQuantity,
            removeItem,
            addItem,
            syncWithSelectedIds,
            selectedDrinkIds,
            isHydrated,
        };
    }, [isHydrated, orderItems, total]);

    if (!isHydrated) {
        return <Loading />;
    }

    return <OrderContext.Provider value={value}>{children}</OrderContext.Provider>;
};

export const useOrderContext = (): OrderContextType => {
    const context = useContext(OrderContext);
    if (!context) throw new Error('useOrderContext must be used within an OrderProvider');
    return context;
};
