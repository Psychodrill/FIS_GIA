use-cachecluster
stop-cachecluster
set-cacheconfig -CacheName Instance -Expirable $false -Eviction None
Set-CacheClusterSecurity -SecurityMode None -ProtectionLevel None
start-cachecluster
