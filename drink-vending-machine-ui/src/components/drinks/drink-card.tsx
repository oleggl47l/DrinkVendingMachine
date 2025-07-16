import {DrinkModel} from "@/app/api/drink-vending-machine";
import Image from 'next/image'

export const DrinkCard = ({
                              drink,
                              isSelected,
                              onSelect
                          }: {
    drink: DrinkModel;
    isSelected: boolean;
    onSelect: (id: number) => void;
}) => {
    const isOutOfStock = drink.quantity === 0;

    const buttonText = isOutOfStock
        ? "Закончился"
        : isSelected
            ? "Выбрано"
            : "Выбрать";

    const buttonClass = isOutOfStock
        ? "bg-gray-300 text-gray-600 disabled"
        : isSelected
            ? "bg-green-600 text-white"
            : "bg-yellow-400 text-black hover:bg-yellow-500";

    return (
        <div className="flex flex-col justify-between h-full bg-white rounded shadow p-4">
            <div>
                <Image
                    src={drink.imageUrl || ""}
                    alt={drink.name || "Напиток"}
                    width={200}
                    height={200}
                    className="object-contain mx-auto mb-4"
                    style={{ height: '12rem' }}
                />
                <div className="text-center">
                    <p className="text-sm mb-1">{drink.name}</p>
                    <p className="text-sm font-semibold mb-2">{drink.price} руб.</p>
                </div>
            </div>
            <button
                className={`w-full py-2 text-sm font-medium rounded ${buttonClass}`}
                onClick={() => onSelect(drink.id!)}
                disabled={isOutOfStock}
            >
                {buttonText}
            </button>
        </div>
    );
};

