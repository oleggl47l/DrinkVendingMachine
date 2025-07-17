'use client';

import {DrinkService} from "@/app/api/drink-vending-machine";
import {ChangeEvent, useRef, useState} from "react";

interface Props {
    onImportSuccess?: () => void;
}

export const DrinkImport = ({onImportSuccess}: Props) => {
    const fileInputRef = useRef<HTMLInputElement>(null);
    const [uploading, setUploading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleFileChange = async (e: ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (!file) return;

        setUploading(true);
        setError(null);

        try {
            await DrinkService.importDrinksFromExcel({
                formData: {file}
            });

            if (onImportSuccess) {
                onImportSuccess();
            }

            if (fileInputRef.current) {
                fileInputRef.current.value = "";
            }

            alert("Импорт успешно завершён");
        } catch (e) {
            console.error("Ошибка при импорте:", e);
            setError("Не удалось импортировать файл.");
        } finally {
            setUploading(false);
        }
    };

    return (
        <div className="flex flex-col">
            <button
                onClick={() => fileInputRef.current?.click()}
                className="bg-gray-200 text-gray-800 px-4 py-3 rounded disabled:opacity-50 hover:bg-gray-300"
                disabled={uploading}
            >
                {uploading ? "Импортируется..." : "Импорт"}
            </button>
            <input
                type="file"
                ref={fileInputRef}
                accept=".xlsx,.xls"
                className="hidden"
                onChange={handleFileChange}
            />
            {error && <span className="text-red-500 text-sm mt-2">{error}</span>}
        </div>
    );
};
