(defblock :name serialize-data :is-top t
	(worker
		:worker-name serialize-data
		:verbs "sd"
	)

	(idle :name keys)
	(alt
		(seq
			(multi-text
				:classes key
				:values "-p" "--provider"
				:alias provider
				:action key)
			(multi-text
				:classes term
				:values "sqlserver" "postgresql"
				:action value)
		)
		(seq
			(multi-text
				:classes key
				:values "-e" "--exclude"
				:alias exclude-table
				:action key)
			(some-text
				:classes term string
				:action value)
		)
		(fallback :name bad-option-or-key)
	)

	(idle :links keys next)

	(some-text
		:classes path string
		:alias connection-string
		:action argument)

	(idle :name options)
	(opt
		(alt
			(multi-text
				:classes key
				:values "-v" "--verbose"
				:alias verbose
				:action option)

			(multi-text
				:classes key
				:values "-q" "--quiet"
				:alias quiet
				:action option)
		)
	)
	(idle :links options next)

	(end)
)
