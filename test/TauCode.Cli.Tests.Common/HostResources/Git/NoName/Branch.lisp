(defblock :name branch :is-top t
	(executor
		:executor-name branch
		:verb "branch"
		:doc "Branch management.")
	(idle :name args)
	(alt
		(seq
			(multi-text
				:classes key
				:values "-d" "--delete"
				:alias delete-branch
				:action key)
			(some-text
				:classes path
				:action value)
		)
	)
	(idle :links args next)
	(end)
)
