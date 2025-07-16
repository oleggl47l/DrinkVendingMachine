import {BrandModel} from "@/app/api/drink-vending-machine";
import React, {useState, useEffect} from "react";

export const DrinkFilters = ({
                                 brands,
                                 selectedBrand,
                                 priceBounds,
                                 selectedRange,
                                 onBrandChange,
                                 onPriceChange
                             }: {
    brands: BrandModel[];
    selectedBrand: number | null;
    priceBounds: [number, number];
    selectedRange: [number, number];
    onBrandChange: (brandId: number | null) => void;
    onPriceChange: (newRange: [number, number]) => void;
}) => {
    const [currentMaxPrice, setCurrentMaxPrice] = useState(selectedRange[1]);

    useEffect(() => {
        setCurrentMaxPrice(selectedRange[1]);
    }, [selectedRange]);

    const handleSliderChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = Number(e.target.value);
        setCurrentMaxPrice(newValue);
    };

    const handleSliderRelease = () => {
        onPriceChange([priceBounds[0], currentMaxPrice]);
    };

    return (
        <div className="bg-white p-6 rounded-lg shadow-md mb-8">
            <h2 className="text-xl font-semibold mb-4">Фильтры</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                        Бренд
                    </label>
                    <select
                        className="w-full p-2 border border-gray-300 rounded-md"
                        value={selectedBrand || ''}
                        onChange={(e) => onBrandChange(e.target.value ? Number(e.target.value) : null)}
                    >
                        <option value="">Все бренды</option>
                        {brands.map((brand) => (
                            <option key={brand.id} value={brand.id}>
                                {brand.name}
                            </option>
                        ))}
                    </select>
                </div>
                <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                        Цена: {priceBounds[0]} - {currentMaxPrice} ₽
                    </label>
                    <div className="flex items-center space-x-4">
                        <input
                            type="range"
                            min={priceBounds[0]}
                            max={priceBounds[1]}
                            value={currentMaxPrice}
                            onChange={handleSliderChange}
                            onMouseUp={handleSliderRelease}
                            onTouchEnd={handleSliderRelease}
                            className="w-full"
                        />
                    </div>
                </div>
            </div>
        </div>
    );
};
