(defblock :name drop-all-tables :is-top t
	(worker
		:worker-name drop-all-tables
		:verb "drop-all-tables"
		:description "Drop all tables of a database."
		:usage-samples (
			"drop-all-tables -p sqlserver -e table1 --exclude table2 Server=.;Database=mydb;Trusted_Connection=True;"
		)
	)

	(idle :name keys)
	(alt
		(seq
			(multi-text
				:classes key
				:values "-p" "--provider"
				:alias provider
				:action key
				:is-mandatory t
				:is-single t)
			(multi-text
				:classes term
				:values "sqlserver" "postgresql"
				:action value
				:description "DB provider identifier"
				:doc-subst "db provider")
		)
		(seq
			(multi-text
				:classes key
				:values "-e" "--exclude"
				:alias exclude-table
				:action key)
			(some-text
				:classes term string
				:action value
				:description "Table to exclude from dropping"
				:doc-subst "table to exclude")
		)
		(fallback :name bad-option-or-key)
	)

	(idle :links keys next)

	(some-text
		:classes path string
		:alias connection-string
		:action argument		
		:description "DB connection string to use"
		:doc-subst "connection string")

	(idle :name options)
	(opt
		(alt
			(multi-text
				:classes key
				:values "-v" "--verbose"
				:alias verbose
				:action option
				:description "Verbose output")

			(multi-text
				:classes key
				:values "-q" "--quiet"
				:alias quiet
				:action option
				:description "Don't show output")
		)
	)
	(idle :links options next)

	(end)
)
