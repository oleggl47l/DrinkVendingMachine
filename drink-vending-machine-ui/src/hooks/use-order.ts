import {useEffect, useState} from 'react';
import {DrinkModel, DrinkService} from '@/app/api/drink-vending-machine';

export interface OrderItem extends DrinkModel {
    quantitySelected: number;
}

export const useOrder = () => {
    const [orderItems, setOrderItems] = useState<OrderItem[]>([]);
    const [total, setTotal] = useState(0);

    useEffect(() => {
        const storedIds = localStorage.getItem('selectedDrinkIds');
        if (storedIds) {
            const ids = JSON.parse(storedIds) as number[];

            async function fetchSelectedDrinks() {
                try {
                    const allDrinks = await DrinkService.getAllDrinks({});
                    const selected = allDrinks.filter(d => ids.includes(d.id!));
                    const order = selected.map(d => ({...d, quantitySelected: 1}));
                    setOrderItems(order);
                } catch (e) {
                    console.error(e);
                }
            }

            void fetchSelectedDrinks();
        }
    }, []);

    useEffect(() => {
        const sum = orderItems.reduce((acc, item) => acc + item.price! * item.quantitySelected, 0);
        setTotal(sum);
    }, [orderItems]);

    const changeQuantity = (id: number, delta: number) => {
        setOrderItems((prev) =>
            prev.map((item) => {
                if (item.id === id) {
                    const newQuantity = item.quantitySelected + delta;
                    if (newQuantity < 1 || newQuantity > item.quantity!) return item;
                    return {...item, quantitySelected: newQuantity};
                }
                return item;
            })
        );
    };

    const removeItem = (id: number) => {
        setOrderItems((prev) => prev.filter((item) => item.id !== id));
    };

    return {orderItems, total, changeQuantity, removeItem};
};
