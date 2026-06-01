
'use client'

import {createContext, ReactNode, useContext, useEffect, useState} from 'react'

type Device = 'mobile' | 'tablet' | 'desktop'

interface DeviceContextType {
    device: Device
    isMobile: boolean
    isTablet: boolean
    isDesktop: boolean
}

const DeviceContext = createContext<DeviceContextType>({
    device: 'desktop',
    isMobile: false,
    isTablet: false,
    isDesktop: true,
})

export function DeviceProvider({ children }: { children: ReactNode }) {
    const [device, setDevice] = useState<Device>('desktop')

    useEffect(() => {
        const update = () => {
            const w = window.innerWidth
            if (w < 768) setDevice('mobile')
            else if (w < 1024) setDevice('tablet')
            else setDevice('desktop')
        }

        update()
        window.addEventListener('resize', update)
        return () => window.removeEventListener('resize', update)
    }, [])

    return (
        <DeviceContext.Provider value={{
            device,
            isMobile: device === 'mobile',
            isTablet: device === 'tablet',
            isDesktop: device === 'desktop',
        }}>
            {children}
        </DeviceContext.Provider>
    )
}

export const useDevice = () => useContext(DeviceContext)