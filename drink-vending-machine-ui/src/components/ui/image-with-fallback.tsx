'use client';

import Image from 'next/image';
import React, {useState} from 'react';

interface ImageWithFallbackProps {
    src: string | null | undefined;
    alt: string;
    width: number;
    height: number;
    className?: string;
    fallbackSrc?: string;
    style?: React.CSSProperties;
}

export const ImageWithFallback = ({
                                      src,
                                      alt,
                                      width,
                                      height,
                                      className = '',
                                      fallbackSrc = '/drink-img-not-found.png',
                                      style,
                                  }: ImageWithFallbackProps) => {
    const [imgSrc, setImgSrc] = useState(src || fallbackSrc);

    return (
        <Image
            src={imgSrc}
            alt={alt}
            width={width}
            height={height}
            className={className}
            style={style}
            onError={() => setImgSrc(fallbackSrc)}
        />
    );
};