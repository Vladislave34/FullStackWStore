import {NextConfig} from 'next';
import createNextIntlPlugin from 'next-intl/plugin';

const nextConfig: NextConfig = {
    images: {
            remotePatterns: [
                {
                    protocol: "https",
                    hostname: "picsum.photos",
                },
                {
                    protocol: "http",
                    hostname: "localhost",
                    port: "5125",
                },
            ],
        },
        async rewrites() {
            return [
                   {
                          source: '/api/:path*',
                          destination: 'http://localhost:5125/api/:path*',
                   },
            ];
        },
};


export default nextConfig
