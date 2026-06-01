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
                    hostname: "192.168.64.3",
                    port: "9000",
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

const withNextIntl = createNextIntlPlugin();
export default withNextIntl(nextConfig);
