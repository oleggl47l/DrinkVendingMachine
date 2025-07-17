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
        <div className="grid md:grid-cols-2 gap-10">
            <div className="flex flex-col w-full">
                <label className="block text-sm mb-1">Выберите бренд</label>
                <select
                    className="w-full p-2 border border-gray-300 rounded"
                    value={selectedBrand || ""}
                    onChange={(e) =>
                        onBrandChange(e.target.value ? Number(e.target.value) : null)
                    }
                >
                    <option value="">Все бренды</option>
                    {brands.map((brand) => (
                        <option key={brand.id} value={brand.id}>
                            {brand.name}
                        </option>
                    ))}
                </select>
            </div>
            <div className="flex flex-col w-full">
                <label className="block text-sm mb-1">Стоимость</label>
                <div className="flex justify-between mb-1">
                    <span className="text-xs text-gray-500">{priceBounds[0]} руб.</span>
                    <span className="text-xs text-gray-500">{currentMaxPrice} руб.</span>
                </div>
                <input
                    type="range"
                    min={priceBounds[0]}
                    max={priceBounds[1]}
                    value={currentMaxPrice}
                    onChange={handleSliderChange}
                    onMouseUp={handleSliderRelease}
                    onTouchEnd={handleSliderRelease}
                    className="flex-1 accent-gray-500 transition-opacity duration-300 ease-in-out hover:opacity-85"
                />
            </div>
        </div>
    );
};