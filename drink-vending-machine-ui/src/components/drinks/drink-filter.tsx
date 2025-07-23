import {BrandModel} from "@/app/api/drink-vending-machine";
import React from "react";
import {usePriceSlider} from "@/hooks/catalog/use-price-slider";

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
    const {
        currentValue,
        showTooltip,
        tooltipPosition,
        sliderRef,
        handleChange,
        hideTooltip,
        showTooltipOnHover
    } = usePriceSlider(selectedRange[1]);

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

            <div className="flex flex-col w-full relative">
                <label className="block text-sm mb-1">Стоимость</label>
                <div className="flex justify-between mb-1">
                    <span className="text-xs text-gray-500">{priceBounds[0]} руб.</span>
                    <span className="text-xs text-gray-500">{priceBounds[1]} руб.</span>
                </div>

                <input
                    ref={sliderRef}
                    type="range"
                    min={priceBounds[0]}
                    max={priceBounds[1]}
                    value={currentValue}
                    onChange={(e) => handleChange(Number(e.target.value))}
                    onMouseUp={() => {
                        hideTooltip();
                        onPriceChange([priceBounds[0], currentValue]);
                    }}
                    onTouchEnd={() => {
                        hideTooltip();
                        onPriceChange([priceBounds[0], currentValue]);
                    }}
                    onMouseEnter={showTooltipOnHover}
                    onMouseLeave={hideTooltip}
                    className="accent-gray-500 transition-opacity duration-300 ease-in-out hover:opacity-85"
                />

                {showTooltip && (
                    <div
                        style={{
                            position: "absolute",
                            top: "65px",
                            left: `${tooltipPosition}%`,
                            transform: "translateX(-50%)",
                            padding: "4px 8px",
                            backgroundColor: "rgba(0,0,0,0.75)",
                            color: "white",
                            borderRadius: "4px",
                            fontSize: "12px",
                            pointerEvents: "none",
                            userSelect: "none",
                            whiteSpace: "nowrap",
                            transition: "opacity 0.2s ease",
                            zIndex: 10,
                        }}
                    >
                        {currentValue} руб.
                    </div>
                )}
            </div>
        </div>
    );
};