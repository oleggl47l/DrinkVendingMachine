import { DrinkModel } from "@/app/api/drink-vending-machine";

export const DrinkCard = ({ drink }: { drink: DrinkModel }) => (
    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow">
        {drink.imageUrl && (
            <img
                src={drink.imageUrl}
                alt={drink.name || 'Напиток'}
                className="w-full h-48 object-cover"
                width={300}
                height={200}
                loading="lazy"
            />
        )}
        <div className="p-4">
            <h3 className="font-semibold text-lg mb-1">{drink.name}</h3>
            <p className="text-gray-600 mb-2">{drink.brandName}</p>
            <div className="flex justify-between items-center">
                <span className="font-bold text-lg">{drink.price} ₽</span>
                <span className="text-sm text-gray-500">
          Осталось: {drink.quantity}
        </span>
            </div>
        </div>
    </div>
);