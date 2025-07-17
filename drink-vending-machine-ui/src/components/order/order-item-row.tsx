'use client';

import Image from 'next/image';
import {OrderItem} from "@/context/order-context";

interface Props {
    item?: OrderItem;
    onChangeQuantity?: (id: number, delta: number) => void;
    onRemove?: (id: number) => void;
    isHeader?: boolean;
}

export const OrderItemRow = ({item, onChangeQuantity, onRemove, isHeader}: Props) => {
    if (isHeader) {
        return (
            <div
                className="grid grid-cols-[2fr_2fr_1fr_1fr] gap-4 items-center font-semibold text-gray-700"
                aria-label="Заголовок корзины с товарами"
            >
                <div>Товар</div>
                <div className="flex justify-center">Количество</div>
                <div className="flex justify-center">Цена</div>
                <div aria-hidden="true"></div>
            </div>
        );
    }

    if (!item || item.id == null || item.price == null) {
        return null;
    }

    const {id, price} = item;

    return (
        <div
            className="grid grid-cols-[2fr_2fr_1fr_1fr] gap-4 items-center mb-8"
            aria-label={`Товар ${item.name}`}
        >
            <div className="flex items-center gap-4">
                <Image
                    src={item.imageUrl || ''}
                    alt={item.name || 'Drink'}
                    width={60}
                    height={60}
                    className="object-contain rounded"
                />
                <span>{item.name}</span>
            </div>

            <div className="flex justify-center items-center gap-2">
                <button
                    onClick={() => onChangeQuantity && onChangeQuantity(id, -1)}
                    className="bg-black text-white w-8 h-8 flex items-center justify-center rounded hover:bg-gray-700"
                    aria-label={`Уменьшить количество ${item.name}`}
                >
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"
                         className="lucide lucide-minus-icon lucide-minus">
                        <path d="M5 12h14"/>
                    </svg>
                </button>
                <div className="w-16 h-8 flex items-center justify-center border rounded">
                    {item.quantitySelected}
                </div>
                <button
                    onClick={() => onChangeQuantity && onChangeQuantity(id, 1)}
                    className="bg-black text-white w-8 h-8 flex items-center justify-center rounded hover:bg-gray-700"
                    aria-label={`Увеличить количество ${item.name}`}
                >
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"
                         className="lucide lucide-plus-icon lucide-plus">
                        <path d="M5 12h14"/>
                        <path d="M12 5v14"/>
                    </svg>
                </button>
            </div>

            <div className="flex justify-center font-semibold">
                {(price * item.quantitySelected)} руб.
            </div>

            <button
                onClick={() => onRemove && onRemove(id)}
                className="text-black hover:text-gray-700 flex justify-end"
                aria-label="Удалить товар"
                title="Удалить товар"
            >
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none"
                     stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"
                     className="lucide lucide-trash2-icon lucide-trash-2">
                    <path d="M10 11v6"/>
                    <path d="M14 11v6"/>
                    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6"/>
                    <path d="M3 6h18"/>
                    <path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
                </svg>
            </button>
        </div>
    );
};
