'use client';

import Image from 'next/image';
import {OrderItem} from '@/hooks/use-order';

interface Props {
    item: OrderItem;
    onChangeQuantity: (id: number, delta: number) => void;
    onRemove: (id: number) => void;
}

export const OrderItemRow = ({item, onChangeQuantity, onRemove}: Props) => {
    const {id, price} = item;

    if (id == null || price == null) {
        return null;
    }

    return (
        <div className="grid grid-cols-[1fr_328px_150px_40px] gap-4 items-center mb-4 h-24">
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

            <div className="flex items-center gap-2">
                <button
                    onClick={() => onChangeQuantity(id, -1)}
                    className="bg-black text-white w-8 h-8 flex items-center justify-center rounded"
                    aria-label={`Уменьшить количество ${item.name}`}
                >
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                         className="lucide lucide-minus-icon lucide-minus">
                        <path d="M5 12h14"/>
                    </svg>
                </button>
                <div className="w-18 h-8 flex items-center justify-center border rounded">
                    {item.quantitySelected}
                </div>
                <button
                    onClick={() => onChangeQuantity(id, 1)}
                    className="bg-black text-white w-8 h-8 flex items-center justify-center rounded"
                    aria-label={`Увеличить количество ${item.name}`}
                >
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                         className="lucide lucide-plus-icon lucide-plus">
                        <path d="M5 12h14"/>
                        <path d="M12 5v14"/>
                    </svg>
                </button>
            </div>

            <div>{(price * item.quantitySelected)} руб.</div>

            <button
                onClick={() => onRemove(id)}
                className="text-black hover:text-gray-700"
                aria-label="Удалить товар"
                title="Удалить товар"
            >
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none"
                     stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
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
